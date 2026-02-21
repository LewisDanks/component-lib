import { z } from "zod";

export const loginPageName = "Login" as const;

export const loginSchema = z.object({
  pageName: z.literal(loginPageName),
  params: z.object({
    returnUrl: z.string().nullable(),
  }),
  state: z.object({
    signInPath: z.string(),
    dashboardPath: z.string(),
    antiForgeryToken: z.string(),
    antiForgeryHeaderName: z.string(),
    demoEmail: z.string(),
    demoPassword: z.string(),
  }),
});
