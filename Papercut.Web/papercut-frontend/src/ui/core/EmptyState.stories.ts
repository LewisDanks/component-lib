import type { Meta, StoryObj } from '@storybook/vue3-vite'
import EmptyState from './EmptyState.vue'

const meta = {
  title: 'Core/EmptyState',
  component: EmptyState,
  tags: ['autodocs'],
  args: {
    title: 'No data',
    message: 'No records are available yet.',
  },
} satisfies Meta<typeof EmptyState>

export default meta
type Story = StoryObj<typeof meta>

export const Default: Story = {}
