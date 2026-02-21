import { z } from "zod";

export const settingsPageName = "Settings" as const;

export const settingsSchema = z.object({
  pageName: z.literal(settingsPageName),
  params: z.object({}),
  state: z.object({
    displayName: z.string(),
    email: z.string(),
    timezone: z.string(),
    timezoneOptions: z.array(
      z.object({
        value: z.string(),
        label: z.string(),
      })
    ),
    marketingEmailsEnabled: z.boolean(),
    twoFactorEnabled: z.boolean(),
    dashboardPath: z.string(),
  }),
});
