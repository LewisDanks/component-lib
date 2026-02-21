import type { PageModule } from "../../app/pageModule";
import SettingsRoot from "./SettingsRoot.vue";
import { settingsPageName, settingsSchema } from "./schema";

export const SettingsPage = {
  pageName: settingsPageName,
  schema: settingsSchema,
  root: SettingsRoot,
} as const satisfies PageModule<typeof settingsPageName, typeof settingsSchema>;
