import type { Meta, StoryObj } from '@storybook/vue3-vite'
import TextInput from './TextInput.vue'

const meta = {
  title: 'Core/TextInput',
  component: TextInput,
  tags: ['autodocs'],
  args: {
    id: 'text-input',
    modelValue: '',
    type: 'text',
    placeholder: 'Enter value',
    disabled: false,
  },
} satisfies Meta<typeof TextInput>

export default meta
type Story = StoryObj<typeof meta>

export const Text: Story = {}

export const Email: Story = {
  args: {
    type: 'email',
    placeholder: 'name@example.com',
  },
}

export const Password: Story = {
  args: {
    type: 'password',
    placeholder: 'Password',
  },
}
