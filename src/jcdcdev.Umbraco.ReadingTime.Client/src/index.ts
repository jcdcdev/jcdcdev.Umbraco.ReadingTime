import {manifests as editors} from './editors/manifest.ts';

import {UMB_AUTH_CONTEXT} from "@umbraco-cms/backoffice/auth";
import {OpenAPI} from "./api";
import {UmbEntryPointOnInit} from "@umbraco-cms/backoffice/extension-api";
import {ReadingTimeContext} from "./context/reading-time.context.ts";

export const onInit: UmbEntryPointOnInit = (_host, extensionRegistry) => {
    new ReadingTimeContext(_host);
    extensionRegistry.registerMany([
        ...editors,
    ]);

    _host.consumeContext(UMB_AUTH_CONTEXT, (_auth) => {
        const umbOpenApi = _auth.getOpenApiConfiguration();
        OpenAPI.TOKEN = umbOpenApi.token;
        OpenAPI.BASE = umbOpenApi.base;
        OpenAPI.WITH_CREDENTIALS = umbOpenApi.withCredentials;
    });
};
