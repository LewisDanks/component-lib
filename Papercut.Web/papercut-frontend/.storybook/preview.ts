import type { Preview } from '@storybook/vue3-vite'

const preview: Preview = {
  parameters: {
    options: {
      storySort: {
        order: ['Core'],
      },
    },
    controls: {
      expanded: true,
    },
  },
}

export default preview
