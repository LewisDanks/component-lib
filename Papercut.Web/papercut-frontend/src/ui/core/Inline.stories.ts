import type { Meta, StoryObj } from '@storybook/vue3-vite'
import Inline from './Inline.vue'
import Button from './Button.vue'

const meta = {
  title: 'Core/Inline',
  component: Inline,
  tags: ['autodocs'],
  args: {
    align: 'center',
    justify: 'start',
    gap: 'sm',
    wrap: true,
  },
  render: (args) => ({
    components: { Inline, Button },
    setup() {
      return { args }
    },
    template:
      "<Inline v-bind='args'><Button variant='secondary'>Back</Button><Button>Save</Button></Inline>",
  }),
} satisfies Meta<typeof Inline>

export default meta
type Story = StoryObj<typeof meta>

export const Default: Story = {}

export const SpaceBetween: Story = {
  args: {
    justify: 'space-between',
    wrap: false,
  },
}
