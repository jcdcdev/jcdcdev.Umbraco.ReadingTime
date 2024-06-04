import {LitElement, html, customElement, property, state} from "@umbraco-cms/backoffice/external/lit";
import {UmbPropertyEditorUiElement} from "@umbraco-cms/backoffice/extension-registry";
import {UmbPropertyEditorConfigCollection} from "@umbraco-cms/backoffice/property-editor";
import {UmbElementMixin} from "@umbraco-cms/backoffice/element-api";
import {UMB_ENTITY_CONTEXT} from "@umbraco-cms/backoffice/entity";
import {UMB_PROPERTY_CONTEXT} from "@umbraco-cms/backoffice/property";
import {UMB_CONTENT_PROPERTY_CONTEXT} from "@umbraco-cms/backoffice/content";
import {ReadingTimeResponse} from "../api";
import {ReadingTimeContext} from "../context/reading-time.context.ts";
import {PropertyValues} from "lit";

@customElement('reading-time-property-editor-ui')
export default class MySuggestionsPropertyEditorUIElement extends UmbElementMixin(LitElement) implements UmbPropertyEditorUiElement {
    @property({type: String})
    public value = "";
    @state()
    private _hideVariationWarning: boolean = false;

    #readingTimeContext?: ReadingTimeContext;

    @state()
    private initialised: boolean = false;
    @state()
    private contentKey: string | null | undefined;
    @state()
    private dataTypeKey: string | null | undefined;
    @state()
    private culture: string | null | undefined;
    @state()
    private data: ReadingTimeResponse | null = null;
    @state()
    private valid: boolean = false;

    constructor() {
        super();
        this.#readingTimeContext = new ReadingTimeContext(this);
        this.consumeContext(UMB_ENTITY_CONTEXT, (context) => {
            this.contentKey = context.getUnique();
        });

        this.consumeContext(UMB_PROPERTY_CONTEXT, (context) => {
            this.culture = context.getVariantId()?.culture;
        });

        this.consumeContext(UMB_CONTENT_PROPERTY_CONTEXT, (context) => {
            context.dataType.subscribe((dataType) => {
                this.dataTypeKey = dataType?.unique
            }).unsubscribe();
        });
    }

    @property({attribute: false})
    public set config(config: UmbPropertyEditorConfigCollection) {
        this._hideVariationWarning = config.getValueByAlias<boolean>("hideVariationWarning") ?? false;
    }

    render() {
        if (!this.initialised) {
            return html
                `
                    <div>Loading...</div>
                `;
        }
        if (!this.valid) {
            const message = this.contentKey ? "No data available" : "Save and publish to calculate reading time";
            return html
                `
                    <div>${message}</div>
                `;
        }
        const message = this.data?.readingTime;
        const alert = this._hideVariationWarning || this.culture !== "" ? "" : html
            `
                <div>Reading time is calculated based on the content of the default culture</div>
            `;

        return html
            `
                <div>
                    ${message}
                    ${alert}
                </div>
            `;
    }

    protected updated(_changedProperties: PropertyValues) {
        if (this.contentKey && this.dataTypeKey && !this.initialised) {
            this.init();
        }
    }

    private async init() {
        const result = await this.#readingTimeContext?.getReadingTime(this.contentKey!, this.dataTypeKey!, this.culture!);
        if (!result) {
            return;
        }
        this.initialised = true;
        this.valid = result.data !== undefined;
        this.data = result.data!;
    }
}

declare global {
    interface HTMLElementTagNameMap {
        'reading-time-property-editor-ui': MySuggestionsPropertyEditorUIElement;
    }
}
