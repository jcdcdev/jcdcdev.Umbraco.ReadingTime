import { defineConfig } from "vite";

export default defineConfig({
  build: {
    lib: {
      entry: "src/index.ts",
      formats: ["es"],
    },
    outDir: "../jcdcdev.Umbraco.ReadingTime/wwwroot/App_Plugins/jcdcdev.Umbraco.ReadingTime/dist/",
    sourcemap: true,
    rollupOptions: {
      external: [/^@umbraco/],
    },
  },
});
