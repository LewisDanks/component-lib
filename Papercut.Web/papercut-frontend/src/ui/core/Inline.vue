<script setup lang="ts">
type InlineElement = "div" | "section" | "article" | "main" | "header" | "footer" | "form";
type InlineAlign = "start" | "center" | "end";
type InlineJustify = "start" | "center" | "space-between";
type InlineGap = "xs" | "sm" | "md" | "lg";

const props = withDefaults(
  defineProps<{
    as?: InlineElement;
    align?: InlineAlign;
    justify?: InlineJustify;
    gap?: InlineGap;
    wrap?: boolean;
  }>(),
  {
    as: "div",
    align: "center",
    justify: "start",
    gap: "sm",
    wrap: true,
  }
);

const gapTokens: Record<InlineGap, string> = {
  xs: "0.25rem",
  sm: "0.5rem",
  md: "1rem",
  lg: "1.5rem",
};

const alignItems: Record<InlineAlign, string> = {
  start: "flex-start",
  center: "center",
  end: "flex-end",
};

const justifyContent: Record<InlineJustify, string> = {
  start: "flex-start",
  center: "center",
  "space-between": "space-between",
};
</script>

<template>
  <component
    :is="props.as"
    data-core="Inline"
    class="pc-inline"
    :style="{
      '--pc-inline-gap': gapTokens[props.gap],
      '--pc-inline-align': alignItems[props.align],
      '--pc-inline-justify': justifyContent[props.justify],
      '--pc-inline-wrap': props.wrap ? 'wrap' : 'nowrap',
    }"
  >
    <slot />
  </component>
</template>

<style scoped>
.pc-inline {
  display: flex;
  align-items: var(--pc-inline-align);
  justify-content: var(--pc-inline-justify);
  gap: var(--pc-inline-gap);
  flex-wrap: var(--pc-inline-wrap);
}
</style>
