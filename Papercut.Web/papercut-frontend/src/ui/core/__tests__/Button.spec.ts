import { mount } from "@vue/test-utils";
import { describe, expect, it } from "vitest";
import Button from "../Button.vue";

describe("Button", () => {
  it("renders slot content", () => {
    const wrapper = mount(Button, {
      slots: {
        default: "Save",
      },
    });

    expect(wrapper.text()).toContain("Save");
    expect(wrapper.attributes("data-core")).toBe("Button");
  });

  it("emits click when enabled", async () => {
    const wrapper = mount(Button, {
      slots: {
        default: "Run",
      },
    });

    await wrapper.get("button").trigger("click");

    expect(wrapper.emitted("click")).toHaveLength(1);
  });

  it("does not emit click when disabled", async () => {
    const wrapper = mount(Button, {
      props: {
        disabled: true,
      },
      slots: {
        default: "Disabled",
      },
    });

    await wrapper.get("button").trigger("click");

    expect(wrapper.emitted("click")).toBeUndefined();
  });
});
