import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig(() => {
  const isStorybook =
    process.env.STORYBOOK === 'true' ||
    process.argv.some((argument) => argument.toLowerCase().includes('storybook'))

  return {
    plugins: [
      vue(),
      ...(isStorybook ? [] : [vueDevTools()]),
    ],
    publicDir: false as const,
    build: {
      manifest: true,
      outDir: '../wwwroot',
      emptyOutDir: false,
      rollupOptions: {
        input: fileURLToPath(new URL('./src/main.ts', import.meta.url)),
      },
    },
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url))
      },
    },
  }
})
