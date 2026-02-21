import { expect, test } from '@playwright/test'

test('routes unauthenticated users to login', async ({ page, context }) => {
  await context.clearCookies()
  await page.goto('/')

  await expect(page).toHaveURL(/\/Login/)
  await expect(page.getByRole('heading', { name: 'Login' })).toBeVisible()
  await expect(page.locator('[data-core="TextInput"]')).toHaveCount(2)
  await expect(page.getByRole('button', { name: 'Sign in' })).toBeVisible()
})

test('signs in and shows localized dashboard clock', async ({ page, context }) => {
  await context.clearCookies()
  await page.goto('/')

  await page.getByRole('button', { name: 'Sign in' }).click()

  await expect(page).toHaveURL(/\/Dashboard/)
  await expect(page.getByRole('heading', { name: 'Dashboard' })).toBeVisible()
  await expect(page.getByText('Hello, Demo User')).toBeVisible()
  await expect(page.getByText('Time zone: UTC')).toBeVisible()
  await expect(page.getByText('Locale: en-GB')).toBeVisible()
  await expect(page.getByTestId('dashboard-clock')).not.toBeEmpty()
})

test('saves localization preferences in settings and reflects on dashboard', async ({ page, context }) => {
  await context.clearCookies()
  await page.goto('/')

  await page.getByRole('button', { name: 'Sign in' }).click()
  await page.getByRole('button', { name: 'Open settings' }).click()

  await expect(page).toHaveURL(/\/Settings/)
  await expect(page.getByRole('heading', { name: 'Settings' })).toBeVisible()

  await page.getByLabel('Time zone').selectOption('America/Los_Angeles')
  await page.getByLabel('Locale').selectOption('en-US')
  await page.getByRole('button', { name: 'Save settings' }).click()

  await expect(page.getByText('Preferences updated.')).toBeVisible()

  await page.getByRole('button', { name: 'Back to dashboard' }).click()

  await expect(page).toHaveURL(/\/Dashboard/)
  await expect(page.getByText('Time zone: America/Los_Angeles')).toBeVisible()
  await expect(page.getByText('Locale: en-US')).toBeVisible()
})
