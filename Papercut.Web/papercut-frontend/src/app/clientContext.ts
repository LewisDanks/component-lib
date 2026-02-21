import { z } from "zod";
import { dashboardPageName, dashboardSchema } from "../pages/dashboard/schema";
import { loginPageName, loginSchema } from "../pages/login/schema";
import { settingsPageName, settingsSchema } from "../pages/settings/schema";

const schemas = [loginSchema, dashboardSchema, settingsSchema] as const;

export const PageNameSchema = z.enum(
  [loginPageName, dashboardPageName, settingsPageName] as const
);

export const ClientContextSchema = z.discriminatedUnion(
  "pageName",
  schemas
);

export type ClientContext = z.infer<typeof ClientContextSchema>;
