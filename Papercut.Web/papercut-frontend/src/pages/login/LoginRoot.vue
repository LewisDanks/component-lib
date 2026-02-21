<script setup lang="ts">
import { ref } from "vue";
import { z } from "zod";
import type { ClientContext } from "../../app/clientContext";
import { Alert, Button, FormField, PageHeader, Stack, Surface, TextInput } from "../../ui/core";

const props = defineProps<{
  context: Extract<ClientContext, { pageName: "Login" }>;
}>();

const SignInResponseSchema = z.object({
  outcome: z.enum(["success", "invalid", "twoFactorRequired"]),
  redirectPath: z.string().nullable(),
  message: z.string().nullable(),
  requiresTwoFactor: z.boolean(),
});

const email = ref(props.context.state.demoEmail);
const password = ref(props.context.state.demoPassword);
const twoFactorCode = ref("");
const requiresTwoFactor = ref(false);
const isSubmitting = ref(false);
const statusMessage = ref<string | null>(null);

async function signIn() {
  isSubmitting.value = true;
  statusMessage.value = null;

  try {
    const formBody = new URLSearchParams();
    formBody.set("returnUrl", props.context.params.returnUrl ?? "");
    formBody.set("email", email.value);
    formBody.set("password", password.value);
    formBody.set("twoFactorCode", twoFactorCode.value);

    const response = await fetch(props.context.state.signInPath, {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
        [props.context.state.antiForgeryHeaderName]: props.context.state.antiForgeryToken,
      },
      body: formBody,
    });

    const rawPayload: unknown = await response.json();
    const parsed = SignInResponseSchema.safeParse(rawPayload);

    if (!parsed.success) {
      statusMessage.value = "Unexpected response from sign-in endpoint.";
      requiresTwoFactor.value = false;
      return;
    }

    if (parsed.data.outcome === "success" && parsed.data.redirectPath) {
      window.location.assign(parsed.data.redirectPath);
      return;
    }

    requiresTwoFactor.value = parsed.data.requiresTwoFactor;
    statusMessage.value = parsed.data.message;
  } catch {
    statusMessage.value = "Sign-in request failed.";
  } finally {
    isSubmitting.value = false;
  }
}
</script>

<template>
  <main>
    <Stack as="section" gap="md">
      <PageHeader title="Login" description="Identity-backed cookie authentication." />

      <Alert
        v-if="statusMessage"
        :tone="requiresTwoFactor ? 'info' : 'error'"
        title="Sign in"
        :message="statusMessage"
      />

      <Surface>
        <Stack as="section" gap="md">
          <FormField id="login-email" label="Email">
            <TextInput id="login-email" v-model="email" type="email" :disabled="isSubmitting" />
          </FormField>

          <FormField id="login-password" label="Password">
            <TextInput id="login-password" v-model="password" type="password" :disabled="isSubmitting" />
          </FormField>

          <FormField
            v-if="requiresTwoFactor"
            id="login-two-factor"
            label="Authenticator code"
            hint="Enter the 6-digit TOTP code from your authenticator app."
          >
            <TextInput id="login-two-factor" v-model="twoFactorCode" :disabled="isSubmitting" />
          </FormField>

          <Button variant="primary" :disabled="isSubmitting" @click="signIn">Sign in</Button>
        </Stack>
      </Surface>
    </Stack>
  </main>
</template>
