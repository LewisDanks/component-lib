import type { PageModule } from "../../app/pageModule";
import LoginRoot from "./LoginRoot.vue";
import { loginPageName, loginSchema } from "./schema";

export const LoginPage = {
  pageName: loginPageName,
  schema: loginSchema,
  root: LoginRoot,
} as const satisfies PageModule<typeof loginPageName, typeof loginSchema>;
