import { DashboardPage } from "../pages/dashboard/page";
import { LoginPage } from "../pages/login/page";
import { SettingsPage } from "../pages/settings/page";

export const pages = [
  LoginPage,
  DashboardPage,
  SettingsPage,
] as const;
