import type { Meta, StoryObj } from '@storybook/vue3-vite'
import Stack from './Stack.vue'
import Surface from './Surface.vue'

const meta = {
  title: 'Core/Stack',
  component: Stack,
  tags: ['autodocs'],
  args: {
    as: 'section',
    gap: 'md',
  },
  render: (args) => ({
    components: { Stack, Surface },
    setup() {
      return { args }
    },
    template:
      "<Stack v-bind='args'><Surface>Block 1</Surface><Surface>Block 2</Surface><Surface>Block 3</Surface></Stack>",
  }),
} satisfies Meta<typeof Stack>

export default meta
type Story = StoryObj<typeof meta>

export const Default: Story = {}

export const LargeGap: Story = {
  args: {
    gap: 'lg',
  },
}
