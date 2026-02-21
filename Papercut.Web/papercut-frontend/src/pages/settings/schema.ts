import { z } from "zod";

export const settingsPageName = "Settings" as const;

export const settingsSchema = z.object({
  pageName: z.literal(settingsPageName),
  params: z.object({}),
  state: z.object({
    displayName: z.string(),
    email: z.string(),
    preferredTimeZoneId: z.string(),
    timeZoneOptions: z.array(
      z.object({
        value: z.string(),
        label: z.string(),
      })
    ),
    preferredCulture: z.string(),
    cultureOptions: z.array(
      z.object({
        value: z.string(),
        label: z.string(),
      })
    ),
    marketingEmailsEnabled: z.boolean(),
    twoFactorEnabled: z.boolean(),
    authenticatorSharedKey: z.string(),
    authenticatorUri: z.string(),
    dashboardPath: z.string(),
    savePreferencesPath: z.string(),
    enableTwoFactorPath: z.string(),
    disableTwoFactorPath: z.string(),
    antiForgeryToken: z.string(),
    antiForgeryHeaderName: z.string(),
  }),
});
