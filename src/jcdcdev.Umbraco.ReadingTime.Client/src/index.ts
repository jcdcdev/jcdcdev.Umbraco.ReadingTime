import { manifests as editors } from './editors/manifest.ts';
import { UmbEntryPointOnInit } from "@umbraco-cms/backoffice/extension-api";

export const onInit: UmbEntryPointOnInit = (_host, extensionRegistry) => {
    extensionRegistry.registerMany([
        ...editors,
    ]);
};
