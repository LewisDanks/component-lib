import { expect, test } from '@playwright/test'

test('routes unauthenticated users to login', async ({ page, context }) => {
  await context.clearCookies()
  await page.goto('/')
  await expect(page).toHaveURL(/\/Login/)
  await expect(page.getByRole('heading', { name: 'Login' })).toBeVisible()
  await expect(page.locator('[data-core="PageHeader"]')).toBeVisible()
  await expect(page.locator('[data-core="Surface"]')).toBeVisible()
  await expect(page.locator('[data-core="Button"]')).toBeVisible()
})

test('dummy login routes to dashboard', async ({ page, context }) => {
  await context.clearCookies()
  await page.goto('/')

  await page.getByRole('button', { name: 'Sign in' }).click()

  await expect(page).toHaveURL(/\/Dashboard/)
  await expect(page.getByRole('heading', { name: 'Dashboard' })).toBeVisible()
  await expect(page.getByText('Hello, Demo User')).toBeVisible()
  await expect(page.locator('[data-core="PageHeader"]')).toBeVisible()
  await expect(page.locator('[data-core="Surface"]')).toBeVisible()
  await expect(page.locator('[data-core="Button"]')).toHaveCount(2)
})

test('settings page exercises shared component library', async ({ page, context }) => {
  await context.clearCookies()
  await page.goto('/')
  await page.getByRole('button', { name: 'Sign in' }).click()
  await page.getByRole('button', { name: 'Open settings' }).click()

  await expect(page).toHaveURL(/\/Settings/)
  await expect(page.getByRole('heading', { name: 'Settings' })).toBeVisible()
  await expect(page.locator('[data-core="PageHeader"]')).toBeVisible()
  await expect(page.locator('[data-core="Surface"]')).toHaveCount(3)
  await expect(page.locator('[data-core="FormField"]')).toHaveCount(3)
  await expect(page.locator('[data-core="TextInput"]')).toHaveCount(2)
  await expect(page.locator('[data-core="SelectInput"]')).toHaveCount(1)
  await expect(page.locator('[data-core="CheckboxInput"]')).toHaveCount(1)
  await expect(page.locator('[data-core="ToggleInput"]')).toHaveCount(1)
  await expect(page.locator('[data-core="Alert"]')).toBeVisible()
  await expect(page.locator('[data-core="EmptyState"]')).toBeVisible()
})
