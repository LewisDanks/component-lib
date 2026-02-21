import type { Meta, StoryObj } from '@storybook/vue3-vite'
import FormField from './FormField.vue'
import TextInput from './TextInput.vue'

const meta = {
  title: 'Core/FormField',
  component: FormField,
  tags: ['autodocs'],
  args: {
    id: 'display-name',
    label: 'Display name',
    hint: 'This name appears on the dashboard.',
  },
  render: (args) => ({
    components: { FormField, TextInput },
    setup() {
      return { args }
    },
    template:
      "<FormField v-bind='args'><TextInput id='display-name' model-value='Demo User' /></FormField>",
  }),
} satisfies Meta<typeof FormField>

export default meta
type Story = StoryObj<typeof meta>

export const WithHint: Story = {}

export const WithError: Story = {
  args: {
    error: 'Display name is required.',
  },
}
