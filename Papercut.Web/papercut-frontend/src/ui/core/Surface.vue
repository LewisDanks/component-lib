<script setup lang="ts">
type SurfaceElement = "div" | "section" | "article";

const props = withDefaults(
  defineProps<{
    as?: SurfaceElement;
    padded?: boolean;
  }>(),
  {
    as: "section",
    padded: true,
  }
);
</script>

<template>
  <component
    :is="props.as"
    data-core="Surface"
    :class="['pc-surface', { 'pc-surface--padded': props.padded }]"
  >
    <slot />
  </component>
</template>

<style scoped>
.pc-surface {
  border: 0.0625rem solid currentColor;
  border-radius: 0.25rem;
}

.pc-surface--padded {
  padding: 1rem;
}
</style>
