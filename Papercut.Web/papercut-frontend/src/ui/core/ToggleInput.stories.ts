import type { Meta, StoryObj } from '@storybook/vue3-vite'
import ToggleInput from './ToggleInput.vue'

const meta = {
  title: 'Core/ToggleInput',
  component: ToggleInput,
  tags: ['autodocs'],
  args: {
    id: 'toggle-input',
    label: 'Enable two-factor authentication',
    modelValue: false,
    disabled: false,
  },
} satisfies Meta<typeof ToggleInput>

export default meta
type Story = StoryObj<typeof meta>

export const Default: Story = {}

export const Enabled: Story = {
  args: {
    modelValue: true,
  },
}
