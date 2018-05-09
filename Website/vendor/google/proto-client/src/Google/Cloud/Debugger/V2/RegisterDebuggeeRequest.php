<?php
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: google/devtools/clouddebugger/v2/controller.proto

namespace Google\Cloud\Debugger\V2;

use Google\Protobuf\Internal\GPBType;
use Google\Protobuf\Internal\RepeatedField;
use Google\Protobuf\Internal\GPBUtil;

/**
 * Request to register a debuggee.
 *
 * Generated from protobuf message <code>google.devtools.clouddebugger.v2.RegisterDebuggeeRequest</code>
 */
class RegisterDebuggeeRequest extends \Google\Protobuf\Internal\Message
{
    /**
     * Debuggee information to register.
     * The fields `project`, `uniquifier`, `description` and `agent_version`
     * of the debuggee must be set.
     *
     * Generated from protobuf field <code>.google.devtools.clouddebugger.v2.Debuggee debuggee = 1;</code>
     */
    private $debuggee = null;

    public function __construct() {
        \GPBMetadata\Google\Devtools\Clouddebugger\V2\Controller::initOnce();
        parent::__construct();
    }

    /**
     * Debuggee information to register.
     * The fields `project`, `uniquifier`, `description` and `agent_version`
     * of the debuggee must be set.
     *
     * Generated from protobuf field <code>.google.devtools.clouddebugger.v2.Debuggee debuggee = 1;</code>
     * @return \Google\Cloud\Debugger\V2\Debuggee
     */
    public function getDebuggee()
    {
        return $this->debuggee;
    }

    /**
     * Debuggee information to register.
     * The fields `project`, `uniquifier`, `description` and `agent_version`
     * of the debuggee must be set.
     *
     * Generated from protobuf field <code>.google.devtools.clouddebugger.v2.Debuggee debuggee = 1;</code>
     * @param \Google\Cloud\Debugger\V2\Debuggee $var
     * @return $this
     */
    public function setDebuggee($var)
    {
        GPBUtil::checkMessage($var, \Google\Cloud\Debugger\V2\Debuggee::class);
        $this->debuggee = $var;

        return $this;
    }

}

