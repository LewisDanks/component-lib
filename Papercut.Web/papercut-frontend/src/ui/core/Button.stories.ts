import type { Meta, StoryObj } from '@storybook/vue3-vite'
import Button from './Button.vue'

const meta = {
  title: 'Core/Button',
  component: Button,
  tags: ['autodocs'],
  args: {
    variant: 'primary',
    type: 'button',
    disabled: false,
  },
  render: (args) => ({
    components: { Button },
    setup() {
      return { args }
    },
    template: "<Button v-bind='args'>Action</Button>",
  }),
} satisfies Meta<typeof Button>

export default meta
type Story = StoryObj<typeof meta>

export const Primary: Story = {}

export const Secondary: Story = {
  args: {
    variant: 'secondary',
  },
}

export const Danger: Story = {
  args: {
    variant: 'danger',
  },
}

export const Disabled: Story = {
  args: {
    disabled: true,
  },
}
