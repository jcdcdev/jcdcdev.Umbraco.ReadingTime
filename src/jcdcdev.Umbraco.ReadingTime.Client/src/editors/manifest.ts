import {ManifestPropertyEditorUi} from "@umbraco-cms/backoffice/property-editor";

const editors: Array<ManifestPropertyEditorUi> = [
    {
        type: "propertyEditorUi",
        alias: "jcdcdev.ReadingTime",
        name: "Reading Time Property Editor UI",
        element: () => import('./reading-time.editor.ts'),
        elementName: "reading-time-property-editor-ui",
        meta: {
            label: "Reading Time",
            icon: "icon-list",
            group: "common",
            propertyEditorSchemaAlias: "jcdcdev.ReadingTime",
            settings: {
                properties: [
                    {
                        alias: "wpm",
                        label: "Words per minute",
                        description: "The average number of words per minute a person can read (studies suggest 150-250)",
                        propertyEditorUiAlias: "Umb.PropertyEditorUi.Integer"
                    },
                    {
                        alias: "minUnit",
                        label: "Minimum unit",
                        description: "The minimum unit of time to display (e.g. seconds, minutes, hours)",
                        propertyEditorUiAlias: "Umb.PropertyEditorUi.Dropdown",
                        config: [
                            {
                                alias: "items",
                                value: [
                                    "Second",
                                    "Minute",
                                    "Hour",
                                    "Day"
                                ]
                            }
                        ]
                    },
                    {
                        alias: "maxUnit",
                        label: "Maximum unit",
                        description: "The maximum unit of time to display (e.g. seconds, minutes, hours)",
                        propertyEditorUiAlias: "Umb.PropertyEditorUi.Dropdown",
                        config: [
                            {
                                alias: "items",
                                value: [
                                    "Second",
                                    "Minute",
                                    "Hour",
                                    "Day"
                                ]
                            }
                        ]
                    },
                    {
                        alias: "hideVariationWarning",
                        label: "Hide variation warning",
                        description: "Hides the warning shown when a content type varies by culture but the data type is invariant",
                        propertyEditorUiAlias: "Umb.PropertyEditorUi.Toggle"
                    }
                ],
                defaultData: [
                    {
                        alias: "wpm",
                        value: 200
                    },
                    {
                        alias: "minUnit",
                        value: "Minute"
                    },
                    {
                        alias: "maxUnit",
                        value: "Minute"
                    },
                    {
                        alias: "hideVariationWarning",
                        value: false
                    }
                ]
            }
        }
    }
]

export const manifests = [...editors];
