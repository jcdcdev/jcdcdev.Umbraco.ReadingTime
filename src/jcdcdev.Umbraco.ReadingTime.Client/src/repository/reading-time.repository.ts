import {UmbControllerHost} from "@umbraco-cms/backoffice/controller-api";
import {UmbDataSourceResponse} from "@umbraco-cms/backoffice/repository";
import {UmbControllerBase} from "@umbraco-cms/backoffice/class-api";
import {type GetUmbracoReadingtimeApiData, ReadingTimeResponse} from "../api";
import {IReadingTimeDataSource, ReadingTimeDataSource} from "./reading-time.datasource.ts";

export class ReadingTimeRepository extends UmbControllerBase {
    #resource: IReadingTimeDataSource;

    constructor(host: UmbControllerHost) {
        super(host);
        this.#resource = new ReadingTimeDataSource(host);
    }

    async getReadingTime(query: GetUmbracoReadingtimeApiData): Promise<UmbDataSourceResponse<ReadingTimeResponse>> {
        return this.#resource.getReadingTime(query);
    }
}

