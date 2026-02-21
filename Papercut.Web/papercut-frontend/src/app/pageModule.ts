import type { Component } from "vue";
import type { ZodTypeAny } from "zod";

export type PageModule<Name extends string, Schema extends ZodTypeAny> = {
  pageName: Name;
  schema: Schema;
  root: Component;
};
