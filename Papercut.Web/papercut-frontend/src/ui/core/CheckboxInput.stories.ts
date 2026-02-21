import type { Meta, StoryObj } from '@storybook/vue3-vite'
import CheckboxInput from './CheckboxInput.vue'

const meta = {
  title: 'Core/CheckboxInput',
  component: CheckboxInput,
  tags: ['autodocs'],
  args: {
    id: 'checkbox-input',
    label: 'Receive marketing emails',
    modelValue: false,
    disabled: false,
  },
} satisfies Meta<typeof CheckboxInput>

export default meta
type Story = StoryObj<typeof meta>

export const Default: Story = {}

export const Checked: Story = {
  args: {
    modelValue: true,
  },
}
