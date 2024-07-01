import {UmbControllerBase} from "@umbraco-cms/backoffice/class-api";
import {UmbControllerHost} from "@umbraco-cms/backoffice/controller-api";
import {UmbDataSourceResponse} from "@umbraco-cms/backoffice/repository";
import {UmbContextToken} from "@umbraco-cms/backoffice/context-api";
import {ReadingTimeResponse} from "../api";
import {ReadingTimeRepository} from "../repository/reading-time.repository.ts";

export class ReadingTimeContext extends UmbControllerBase {
    #repository: ReadingTimeRepository;

    constructor(host: UmbControllerHost) {
        super(host);
        this.#repository = new ReadingTimeRepository(this);
        this.provideContext(READING_TIME_CONTEXT_TOKEN, this);
    }

    async getReadingTime(contentKey: string, dataTypeKey: string, culture?: string): Promise<UmbDataSourceResponse<ReadingTimeResponse>> {
        const query = {
            contentKey: contentKey,
            dataTypeKey: dataTypeKey,
            culture: culture ?? ""
        };
        return await this.#repository.getReadingTime(query);
    }
}

export const READING_TIME_CONTEXT_TOKEN =
    new UmbContextToken<ReadingTimeContext>("BackofficeOrganiserContext");
