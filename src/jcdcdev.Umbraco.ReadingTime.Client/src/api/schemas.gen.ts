// This file is auto-generated by @hey-api/openapi-ts

export const $ReadingTimeResponse = {
    required: ['readingTime', 'updateDate'],
    type: 'object',
    properties: {
        updateDate: {
            type: 'string',
            format: 'date-time'
        },
        readingTime: {
            type: 'string'
        }
    },
    additionalProperties: false
} as const;