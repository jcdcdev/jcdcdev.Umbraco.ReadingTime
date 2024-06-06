import {LitElement, html, customElement, property, state} from "@umbraco-cms/backoffice/external/lit";
import {UmbPropertyEditorUiElement} from "@umbraco-cms/backoffice/extension-registry";
import {UmbPropertyEditorConfigCollection} from "@umbraco-cms/backoffice/property-editor";
import {UmbElementMixin} from "@umbraco-cms/backoffice/element-api";
import {UMB_ENTITY_CONTEXT} from "@umbraco-cms/backoffice/entity";
import {UMB_PROPERTY_CONTEXT} from "@umbraco-cms/backoffice/property";
import {UMB_CONTENT_PROPERTY_CONTEXT} from "@umbraco-cms/backoffice/content";
import {ReadingTimeResponse} from "../api";
import {ReadingTimeContext} from "../context/reading-time.context.ts";
import {css, nothing, PropertyValues} from "lit";
import {UMB_ACTION_EVENT_CONTEXT} from "@umbraco-cms/backoffice/action";
import {UmbRequestReloadStructureForEntityEvent} from "@umbraco-cms/backoffice/entity-action";

@customElement('reading-time-property-editor-ui')
export default class ReadingTimePropertyEditorUi extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {

    @property({type: String})
    public value = "";

    #readingTimeContext?: ReadingTimeContext;

    @state()
    private hideVariationWarning: boolean = false;
    @state()
    private loading: boolean = false;
    @state()
    private contentKey?: string;
    @state()
    private dataTypeKey?: string;
    @state()
    private culture?: string;
    @state()
    private data?: ReadingTimeResponse;
    @state()
    private initialised: boolean = false
    static styles = [css`
        .alert {
            background-color: darkgoldenrod;
            padding: 5px;
        }

        .icon-container {
            display: flex;
            align-items: center;
        }

        .icon {
            margin-right: 5px;
        }
    `]

    constructor() {
        super();
        this.#readingTimeContext = new ReadingTimeContext(this);
        this.consumeContext(UMB_ENTITY_CONTEXT, (context) => {
            this.contentKey = context.getUnique() ?? undefined;
        });

        this.consumeContext(UMB_PROPERTY_CONTEXT, (context) => {
            this.culture = context.getVariantId()?.culture ?? undefined;
        });

        this.consumeContext(UMB_ACTION_EVENT_CONTEXT, (context) => {
            context.addEventListener(UmbRequestReloadStructureForEntityEvent.TYPE, () => {
                if (!this.initialised) {
                    return;
                }
                this.loading = true;
                const interval = setInterval(async () => {
                    if (!(this.contentKey && this.dataTypeKey)) {
                        return;
                    }

                    const response = await this.#readingTimeContext?.getReadingTime(this.contentKey, this.dataTypeKey, this.culture);
                    if (!response || !response.data?.updateDate) {
                        return;
                    }

                    if (response.data.updateDate === this.data?.updateDate) {
                        return;
                    }

                    this.data = response.data;
                    this.loading = false;
                    clearInterval(interval);
                }, 2500);
            });
        });

        this.consumeContext(UMB_CONTENT_PROPERTY_CONTEXT, (context) => {
            context.dataType.subscribe((dataType) => {
                this.dataTypeKey = dataType?.unique
            }).unsubscribe();
        });
    }

    @property({attribute: false})
    public set config(config: UmbPropertyEditorConfigCollection) {
        this.hideVariationWarning = config.getValueByAlias<boolean>("hideVariationWarning") ?? false;
    }

    render() {
        if (this.loading) {
            return html
                `
                    <uui-loader></uui-loader>
                `;
        }

        if (!this.data) {
            return html
                `
                    <div>Save and publish to calculate reading time</div>
                `;
        }

        const alert = this.renderVariationAlert();
        return html
            `
                <div>
                    ${alert}
                    ${this.data.readingTime}
                </div>
            `;
    }

    renderVariationAlert() {
        debugger
        if (this.hideVariationWarning || this.culture) {
            return nothing;
        }

        return html
            `
                <div class="alert">
                    <div class="icon-container">
                        <uui-icon name="alert" class="icon"></uui-icon>
                        <span>Language specific properties are not used in this calculation</span>
                    </div>
                </div>
            `;
    }

    protected updated(_changedProperties: PropertyValues) {
        if (!this.initialised) {
            if (this.contentKey && this.dataTypeKey) {
                this.init();
            }
        }
    }

    private async init() {
        if (this.initialised) {
            return;
        }

        this.loading = true;
        const result = await this.#readingTimeContext?.getReadingTime(this.contentKey!, this.dataTypeKey!, this.culture!);
        this.loading = false;
        this.initialised = true;

        if (!result) {
            return;
        }

        this.data = result.data;
    }
}

declare global {
    interface HTMLElementTagNameMap {
        'reading-time-property-editor-ui': ReadingTimePropertyEditorUi;
    }
}
