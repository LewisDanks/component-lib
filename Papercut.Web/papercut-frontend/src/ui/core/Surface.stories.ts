import type { Meta, StoryObj } from '@storybook/vue3-vite'
import Surface from './Surface.vue'

const meta = {
  title: 'Core/Surface',
  component: Surface,
  tags: ['autodocs'],
  args: {
    as: 'section',
    padded: true,
  },
  render: (args) => ({
    components: { Surface },
    setup() {
      return { args }
    },
    template: "<Surface v-bind='args'>Content container</Surface>",
  }),
} satisfies Meta<typeof Surface>

export default meta
type Story = StoryObj<typeof meta>

export const Padded: Story = {}

export const Unpadded: Story = {
  args: {
    padded: false,
  },
}
