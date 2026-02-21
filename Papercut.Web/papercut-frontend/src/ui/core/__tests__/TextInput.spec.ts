import { mount } from "@vue/test-utils";
import { describe, expect, it } from "vitest";
import TextInput from "../TextInput.vue";

describe("TextInput", () => {
  it("emits model updates on input", async () => {
    const wrapper = mount(TextInput, {
      props: {
        id: "name",
        modelValue: "Demo",
      },
    });

    await wrapper.get("input").setValue("Updated");

    expect(wrapper.emitted("update:modelValue")?.[0]).toEqual(["Updated"]);
  });

  it("respects disabled state", () => {
    const wrapper = mount(TextInput, {
      props: {
        id: "name",
        modelValue: "Demo",
        disabled: true,
      },
    });

    expect(wrapper.get("input").attributes("disabled")).toBeDefined();
  });
});
