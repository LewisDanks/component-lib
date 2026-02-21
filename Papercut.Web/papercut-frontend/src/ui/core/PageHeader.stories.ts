import type { Meta, StoryObj } from '@storybook/vue3-vite'
import PageHeader from './PageHeader.vue'
import Button from './Button.vue'

const meta = {
  title: 'Core/PageHeader',
  component: PageHeader,
  tags: ['autodocs'],
  args: {
    title: 'Settings',
    description: 'Manage profile and localization preferences.',
  },
  render: (args) => ({
    components: { PageHeader, Button },
    setup() {
      return { args }
    },
    template:
      "<PageHeader v-bind='args'><template #actions><Button variant='secondary'>Close</Button></template></PageHeader>",
  }),
} satisfies Meta<typeof PageHeader>

export default meta
type Story = StoryObj<typeof meta>

export const Default: Story = {}
