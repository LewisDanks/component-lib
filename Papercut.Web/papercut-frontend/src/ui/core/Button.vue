<script setup lang="ts">
import type { ButtonVariant } from "./types";

const props = withDefaults(
  defineProps<{
    variant?: ButtonVariant;
    type?: "button" | "submit" | "reset";
    disabled?: boolean;
  }>(),
  {
    variant: "primary",
    type: "button",
    disabled: false,
  }
);

const emit = defineEmits<{
  (e: "click", event: MouseEvent): void;
}>();

function onClick(event: MouseEvent) {
  if (props.disabled) {
    return;
  }

  emit("click", event);
}
</script>

<template>
  <button
    data-core="Button"
    :data-variant="props.variant"
    class="pc-button"
    :class="`pc-button--${props.variant}`"
    :type="props.type"
    :disabled="props.disabled"
    @click="onClick"
  >
    <slot />
  </button>
</template>

<style scoped>
.pc-button {
  border: 0.0625rem solid currentColor;
  border-radius: 0.25rem;
  background: transparent;
  padding: 0.5rem 0.75rem;
  cursor: pointer;
}

.pc-button--primary {
  font-weight: 600;
}

.pc-button--danger {
  text-decoration: underline;
}

.pc-button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
