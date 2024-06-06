import { defineConfig } from '@hey-api/openapi-ts';

export default defineConfig({
	input:
		'http://localhost:54813/umbraco/swagger/default/swagger.json',
	output: {
		format: 'prettier',
		path: './src/api',
	},
	types: {
		enums: 'typescript',
	},
});
