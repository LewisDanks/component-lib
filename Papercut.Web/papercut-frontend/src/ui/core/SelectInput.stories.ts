import type { Meta, StoryObj } from '@storybook/vue3-vite'
import SelectInput from './SelectInput.vue'

const options = [
  { value: 'UTC', label: 'UTC' },
  { value: 'Europe/London', label: 'Europe/London' },
  { value: 'America/Los_Angeles', label: 'America/Los_Angeles' },
]

const meta = {
  title: 'Core/SelectInput',
  component: SelectInput,
  tags: ['autodocs'],
  args: {
    id: 'time-zone',
    modelValue: 'UTC',
    options,
    disabled: false,
  },
} satisfies Meta<typeof SelectInput>

export default meta
type Story = StoryObj<typeof meta>

export const Default: Story = {}

export const Disabled: Story = {
  args: {
    disabled: true,
  },
}
