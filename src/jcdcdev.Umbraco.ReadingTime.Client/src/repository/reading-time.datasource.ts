import { UmbControllerHost } from "@umbraco-cms/backoffice/controller-api";
import { UmbDataSourceResponse } from "@umbraco-cms/backoffice/repository";
import { tryExecute } from "@umbraco-cms/backoffice/resources";
import { ReadingTimeResponse, ReadingTime } from "../api";

export class ReadingTimeDataSource implements IReadingTimeDataSource {

    #host: UmbControllerHost;

    constructor(host: UmbControllerHost) {
        this.#host = host;
    }

    async getReadingTime(contentKey: string, dataTypeKey: string, culture?: string): Promise<UmbDataSourceResponse<ReadingTimeResponse>> {
        return await tryExecute(this.#host, ReadingTime.getUmbracoReadingTimeApiV1({
            query: {
                contentKey: contentKey,
                dataTypeKey: dataTypeKey,
                culture: culture,
            }
        }))
    }

}

export interface IReadingTimeDataSource {
    getReadingTime(contentKey: string, dataTypeKey: string, culture?: string): Promise<UmbDataSourceResponse<ReadingTimeResponse>>;
}

