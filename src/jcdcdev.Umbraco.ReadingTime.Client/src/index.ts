import {manifests as editors} from './editors/manifest.ts';
import {UMB_AUTH_CONTEXT} from "@umbraco-cms/backoffice/auth";
import {UmbEntryPointOnInit} from "@umbraco-cms/backoffice/extension-api";
import {ReadingTimeContext} from "./context/reading-time.context.ts";
import {client} from './api';

export const onInit: UmbEntryPointOnInit = (_host, extensionRegistry) => {
    extensionRegistry.registerMany([
        ...editors,
    ]);

    _host.consumeContext(UMB_AUTH_CONTEXT, (_auth) => {
        if (!_auth) {
            console.error('No auth context found');
            return;
        }

        const config = _auth.getOpenApiConfiguration();
        client.setConfig({
            auth: config.token,
            baseUrl: config.base,
            credentials: config.credentials,
        });

        client.interceptors.request.use(async (request, _options) => {
            const token = await _auth.getLatestToken();
            request.headers.set('Authorization', `Bearer ${token}`);
            return request;
        });

        new ReadingTimeContext(_host);
    });
};
