import { mount } from "@vue/test-utils";
import { describe, expect, it } from "vitest";
import FormField from "../FormField.vue";

describe("FormField", () => {
  it("renders label and hint", () => {
    const wrapper = mount(FormField, {
      props: {
        id: "displayName",
        label: "Display name",
        hint: "Used in greetings",
      },
      slots: {
        default: "<input id=\"displayName\" />",
      },
    });

    expect(wrapper.get("label").text()).toBe("Display name");
    expect(wrapper.text()).toContain("Used in greetings");
  });

  it("renders error state with alert semantics", () => {
    const wrapper = mount(FormField, {
      props: {
        id: "email",
        label: "Email",
        error: "Email is required",
      },
      slots: {
        default: "<input id=\"email\" />",
      },
    });

    const alert = wrapper.get('[role="alert"]');
    expect(alert.text()).toBe("Email is required");
  });
});
