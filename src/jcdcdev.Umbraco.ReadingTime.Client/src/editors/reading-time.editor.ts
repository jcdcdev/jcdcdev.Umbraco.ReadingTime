import { LitElement, html, css, customElement, property, state, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbPropertyEditorUiElement } from '@umbraco-cms/backoffice/property-editor';
import type { UmbPropertyEditorConfigCollection } from '@umbraco-cms/backoffice/property-editor';
import { UMB_PROPERTY_CONTEXT } from '@umbraco-cms/backoffice/property';

const UNIT_ORDER = ['second', 'minute', 'hour', 'day'] as const;
type TimeUnit = (typeof UNIT_ORDER)[number];

@customElement('reading-time-property-editor-ui')
export default class ReadingTimePropertyEditorUi extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {
  @property({ type: Number })
  public value?: number;

  @state()
  private _minUnit: TimeUnit = 'minute';

  @state()
  private _maxUnit: TimeUnit = 'hour';

  @state()
  private _hideVariationWarning: boolean = false;

  @state()
  private _culture?: string;

  constructor() {
    super();
    this.consumeContext(UMB_PROPERTY_CONTEXT, (context) => {
      this._culture = context?.getVariantId()?.culture ?? undefined;
    });
  }

  @property({ attribute: false })
  public set config(config: UmbPropertyEditorConfigCollection) {
    const minVal = config.getValueByAlias<string[]>('minUnit');
    const maxVal = config.getValueByAlias<string[]>('maxUnit');
    this._hideVariationWarning = config.getValueByAlias<boolean>('hideVariationWarning') ?? false;

    const resolvedMin = Array.isArray(minVal) ? minVal[0] : minVal;
    const resolvedMax = Array.isArray(maxVal) ? maxVal[0] : maxVal;

    const normalizedMin = resolvedMin?.toLowerCase() as TimeUnit | undefined;
    const normalizedMax = resolvedMax?.toLowerCase() as TimeUnit | undefined;

    if (normalizedMin && UNIT_ORDER.includes(normalizedMin)) {
      this._minUnit = normalizedMin;
    }
    if (normalizedMax && UNIT_ORDER.includes(normalizedMax)) {
      this._maxUnit = normalizedMax;
    }
  }

  #formatTime(totalSeconds: number): string {
    if (totalSeconds <= 0) {
      return this._minUnit === 'second' ? 'Less than a second' : 'Less than a minute';
    }

    const minIdx = UNIT_ORDER.indexOf(this._minUnit);
    const maxIdx = UNIT_ORDER.indexOf(this._maxUnit);

    const days = Math.floor(totalSeconds / 86400);
    const hours = Math.floor((totalSeconds % 86400) / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;

    const allUnits: { unit: TimeUnit; value: number }[] = [
      { unit: 'day', value: days },
      { unit: 'hour', value: hours },
      { unit: 'minute', value: minutes },
      { unit: 'second', value: seconds },
    ];

    // Filter to only units within the min/max range
    const filtered = allUnits.filter((u) => {
      const idx = UNIT_ORDER.indexOf(u.unit);
      return idx >= minIdx && idx <= maxIdx;
    });

    // If min unit is above seconds, round up the smallest visible unit
    if (minIdx > 0 && filtered.length > 0) {
      const belowMinUnits = allUnits.filter((u) => UNIT_ORDER.indexOf(u.unit) < minIdx);
      const hasRemainder = belowMinUnits.some((u) => u.value > 0);
      if (hasRemainder) {
        const smallest = filtered[filtered.length - 1];
        smallest.value += 1;
        // Handle carry-over
        for (let i = filtered.length - 1; i > 0; i--) {
          const current = filtered[i];
          const parent = filtered[i - 1];
          const limit = current.unit === 'second' ? 60 : current.unit === 'minute' ? 60 : current.unit === 'hour' ? 24 : Infinity;
          if (current.value >= limit) {
            current.value -= limit;
            parent.value += 1;
          }
        }
      }
    }

    const labels: Record<TimeUnit, [string, string]> = {
      day: ['day', 'days'],
      hour: ['hour', 'hours'],
      minute: ['minute', 'minutes'],
      second: ['second', 'seconds'],
    };

    const parts = filtered
      .filter((u) => u.value > 0)
      .map((u) => `${u.value} ${u.value === 1 ? labels[u.unit][0] : labels[u.unit][1]}`);

    if (parts.length === 0) {
      const minLabel = this._minUnit === 'second' ? 'a second' : this._minUnit === 'minute' ? 'a minute' : this._minUnit === 'hour' ? 'an hour' : 'a day';
      return `Less than ${minLabel}`;
    }

    return parts.join(', ');
  }

  #renderVariationAlert() {
    if (this._hideVariationWarning || this._culture) {
      return nothing;
    }

    return html`
      <div class="alert">
        <div class="icon-container">
          <uui-icon name="alert" class="icon"></uui-icon>
          <span>Language specific properties are not used in this calculation</span>
        </div>
      </div>
    `;
  }

  render() {
    if (this.value == null) {
      return html`<em>Reading time will be calculated on save.</em>`;
    }

    return html`
      <div>
        ${this.#renderVariationAlert()}
        <span>${this.#formatTime(this.value)}</span>
      </div>
    `;
  }

  static styles = css`
    :host {
      display: block;
    }
    em {
      color: var(--uui-color-text-alt);
    }
    .alert {
      background-color: darkgoldenrod;
      padding: 5px;
      margin-bottom: 5px;
    }
    .icon-container {
      display: flex;
      align-items: center;
    }
    .icon {
      margin-right: 5px;
    }
  `;
}

declare global {
  interface HTMLElementTagNameMap {
    'reading-time-property-editor-ui': ReadingTimePropertyEditorUi;
  }
}
