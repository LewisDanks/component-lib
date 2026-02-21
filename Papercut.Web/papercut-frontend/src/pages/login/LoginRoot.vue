<script setup lang="ts">
import { ref } from "vue";
import type { ClientContext } from "../../app/clientContext";
import { Button, FormField, PageHeader, Stack, Surface, TextInput } from "../../ui/core";

const props = defineProps<{
  context: Extract<ClientContext, { pageName: "Login" }>;
}>();

const displayName = ref(props.context.state.defaultDisplayName);

function signIn() {
  const trimmedDisplayName = displayName.value.trim();
  const resolvedDisplayName =
    trimmedDisplayName.length > 0 ? trimmedDisplayName : props.context.state.defaultDisplayName;

  const signInUrl = new URL(props.context.state.signInPath, window.location.origin);
  signInUrl.searchParams.set("displayName", resolvedDisplayName);

  window.location.assign(`${signInUrl.pathname}${signInUrl.search}${signInUrl.hash}`);
}
</script>

<template>
  <main>
    <Stack as="section" gap="md">
      <PageHeader title="Login" description="Dummy login flow for scaffolding." />

      <Surface>
        <Stack as="section" gap="md">
          <FormField id="displayName" label="Display name" hint="This is a local demo-only value.">
            <TextInput id="displayName" v-model="displayName" />
          </FormField>

          <Button variant="primary" @click="signIn">Sign in</Button>
        </Stack>
      </Surface>
    </Stack>
  </main>
</template>
