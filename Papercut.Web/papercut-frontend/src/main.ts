import { bootstrapApplication } from "./app/bootstrap";

declare global {
  interface Window {
    papercut: { bootstrapApplication: (context: unknown) => void };
  }
}

window.papercut = { bootstrapApplication };
