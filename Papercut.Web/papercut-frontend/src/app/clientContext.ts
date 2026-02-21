import { z } from "zod";
import { dashboardPageName, dashboardSchema } from "../pages/dashboard/schema";
import { loginPageName, loginSchema } from "../pages/login/schema";

const schemas = [loginSchema, dashboardSchema] as const;

export const PageNameSchema = z.enum(
  [loginPageName, dashboardPageName] as const
);

export const ClientContextSchema = z.discriminatedUnion(
  "pageName",
  schemas
);

export type ClientContext = z.infer<typeof ClientContextSchema>;
