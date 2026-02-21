<script setup lang="ts">
import type { SelectOption } from "./types";

const props = defineProps<{
  id: string;
  modelValue: string;
  options: ReadonlyArray<SelectOption>;
  disabled?: boolean;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: string): void;
}>();

function onChange(event: Event) {
  const target = event.target as HTMLSelectElement;
  emit("update:modelValue", target.value);
}
</script>

<template>
  <select
    :id="props.id"
    data-core="SelectInput"
    class="pc-select-input"
    :value="props.modelValue"
    :disabled="props.disabled"
    @change="onChange"
  >
    <option v-for="option in props.options" :key="option.value" :value="option.value">
      {{ option.label }}
    </option>
  </select>
</template>

<style scoped>
.pc-select-input {
  border: 0.0625rem solid currentColor;
  border-radius: 0.25rem;
  padding: 0.5rem;
}
</style>
