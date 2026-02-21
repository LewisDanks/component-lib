import type { Meta, StoryObj } from '@storybook/vue3-vite'
import LoadingState from './LoadingState.vue'

const meta = {
  title: 'Core/LoadingState',
  component: LoadingState,
  tags: ['autodocs'],
  args: {
    label: 'Loading settings...',
  },
} satisfies Meta<typeof LoadingState>

export default meta
type Story = StoryObj<typeof meta>

export const Default: Story = {}
