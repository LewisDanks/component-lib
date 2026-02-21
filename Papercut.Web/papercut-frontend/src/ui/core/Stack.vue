<script setup lang="ts">
type StackElement = "div" | "section" | "article" | "main" | "header" | "footer" | "form";
type StackGap = "xs" | "sm" | "md" | "lg";

const props = withDefaults(
  defineProps<{
    as?: StackElement;
    gap?: StackGap;
  }>(),
  {
    as: "div",
    gap: "md",
  }
);

const gapTokens: Record<StackGap, string> = {
  xs: "0.25rem",
  sm: "0.5rem",
  md: "1rem",
  lg: "1.5rem",
};
</script>

<template>
  <component
    :is="props.as"
    data-core="Stack"
    class="pc-stack"
    :style="{ '--pc-stack-gap': gapTokens[props.gap] }"
  >
    <slot />
  </component>
</template>

<style scoped>
.pc-stack {
  display: flex;
  flex-direction: column;
  gap: var(--pc-stack-gap);
}
</style>
