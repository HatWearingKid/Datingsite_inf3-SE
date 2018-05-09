<?php
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: google/monitoring/v3/uptime.proto

namespace Google\Cloud\Monitoring\V3;

use Google\Protobuf\Internal\GPBType;
use Google\Protobuf\Internal\RepeatedField;
use Google\Protobuf\Internal\GPBUtil;

/**
 * Used to perform string matching. Currently, this matches on the exact
 * content. In the future, it can be expanded to allow for regular expressions
 * and more complex matching.
 *
 * Generated from protobuf message <code>google.monitoring.v3.UptimeCheckConfig.ContentMatcher</code>
 */
class UptimeCheckConfig_ContentMatcher extends \Google\Protobuf\Internal\Message
{
    /**
     * String content to match
     *
     * Generated from protobuf field <code>string content = 1;</code>
     */
    private $content = '';

    public function __construct() {
        \GPBMetadata\Google\Monitoring\V3\Uptime::initOnce();
        parent::__construct();
    }

    /**
     * String content to match
     *
     * Generated from protobuf field <code>string content = 1;</code>
     * @return string
     */
    public function getContent()
    {
        return $this->content;
    }

    /**
     * String content to match
     *
     * Generated from protobuf field <code>string content = 1;</code>
     * @param string $var
     * @return $this
     */
    public function setContent($var)
    {
        GPBUtil::checkString($var, True);
        $this->content = $var;

        return $this;
    }

}

