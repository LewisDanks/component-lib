<script setup lang="ts">
const props = withDefaults(
  defineProps<{
    id: string;
    modelValue: string;
    type?: "text" | "email" | "password";
    placeholder?: string;
    disabled?: boolean;
  }>(),
  {
    type: "text",
    placeholder: "",
    disabled: false,
  }
);

const emit = defineEmits<{
  (e: "update:modelValue", value: string): void;
}>();

function onInput(event: Event) {
  const target = event.target as HTMLInputElement;
  emit("update:modelValue", target.value);
}
</script>

<template>
  <input
    :id="props.id"
    data-core="TextInput"
    class="pc-text-input"
    :type="props.type"
    :value="props.modelValue"
    :placeholder="props.placeholder"
    :disabled="props.disabled"
    @input="onInput"
  />
</template>

<style scoped>
.pc-text-input {
  border: 0.0625rem solid currentColor;
  border-radius: 0.25rem;
  padding: 0.5rem;
}
</style>
