import type { Meta, StoryObj } from '@storybook/vue3-vite'
import Alert from './Alert.vue'

const meta = {
  title: 'Core/Alert',
  component: Alert,
  tags: ['autodocs'],
  args: {
    tone: 'info',
    title: 'Status',
    message: 'Informational state.',
  },
} satisfies Meta<typeof Alert>

export default meta
type Story = StoryObj<typeof meta>

export const Info: Story = {}

export const Success: Story = {
  args: {
    tone: 'success',
    title: 'Saved',
    message: 'Preferences updated.',
  },
}

export const Error: Story = {
  args: {
    tone: 'error',
    title: 'Error',
    message: 'Unable to complete the request.',
  },
}
