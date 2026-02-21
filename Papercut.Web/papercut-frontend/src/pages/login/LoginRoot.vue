<script setup lang="ts">
import type { ClientContext } from "../../app/clientContext";

const props = defineProps<{
  context: Extract<ClientContext, { pageName: "Login" }>;
}>();

function signIn() {
  window.location.assign(props.context.state.signInPath);
}
</script>

<template>
  <main>
    <h1>Login</h1>
    <p>Dummy login page.</p>
    <button type="button" @click="signIn">
      Sign in as {{ props.context.state.defaultDisplayName }}
    </button>
  </main>
</template>
