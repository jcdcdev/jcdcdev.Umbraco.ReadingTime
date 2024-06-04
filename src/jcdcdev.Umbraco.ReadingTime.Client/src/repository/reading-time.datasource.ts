import {UmbControllerHost} from "@umbraco-cms/backoffice/controller-api";
import {UmbDataSourceResponse} from "@umbraco-cms/backoffice/repository";
import {tryExecuteAndNotify} from "@umbraco-cms/backoffice/resources";
import {getUmbracoReadingtimeApi, type GetUmbracoReadingtimeApiData, ReadingTimeResponse} from "../api";

export class ReadingTimeDataSource implements IReadingTimeDataSource {

    #host: UmbControllerHost;

    constructor(host: UmbControllerHost) {
        this.#host = host;
    }

    async getReadingTime(query: GetUmbracoReadingtimeApiData): Promise<UmbDataSourceResponse<ReadingTimeResponse>> {
        return await tryExecuteAndNotify(this.#host, getUmbracoReadingtimeApi(query))
    }

}

export interface IReadingTimeDataSource {
    getReadingTime(query: GetUmbracoReadingtimeApiData): Promise<UmbDataSourceResponse<ReadingTimeResponse>>;
}

