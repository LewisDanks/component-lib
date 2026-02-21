import { expect, test } from '@playwright/test'

test('routes unauthenticated users to login', async ({ page, context }) => {
  await context.clearCookies()
  await page.goto('/')
  await expect(page).toHaveURL(/\/Login/)
  await expect(page.getByRole('heading', { name: 'Login' })).toBeVisible()
})

test('dummy login routes to dashboard', async ({ page, context }) => {
  await context.clearCookies()
  await page.goto('/')

  await page.getByRole('button', { name: 'Sign in as Demo User' }).click()

  await expect(page).toHaveURL(/\/Dashboard/)
  await expect(page.getByRole('heading', { name: 'Dashboard' })).toBeVisible()
  await expect(page.getByText('Hello, Demo User')).toBeVisible()
})
